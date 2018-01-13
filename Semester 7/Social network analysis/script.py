import os
import csv
import time
import numpy
import datetime
from statistics import median, mean
from collections import defaultdict
from scipy.sparse import coo_matrix, csr_matrix


def print_time(start_time):
    print("Time:", datetime.timedelta(seconds=time.time() - start_time), "\n")


def load_csr(path):
    loaded = numpy.load(path + '.npz')

    return csr_matrix((loaded['data'], loaded['indices'], loaded['indptr']), shape=loaded['shape'])


data_path = "./data/"
graph_path = os.path.join(data_path, "graph")
demography_path = os.path.join(data_path, "trainDemography")
test_users_path = os.path.join(data_path, "users")
results_path = "./results"

if not os.path.exists(results_path):
    os.makedirs(results_path)


print("Loading users...")

start = time.time()
test_users = set()
csv_reader = csv.reader(open(test_users_path))

for line in csv_reader:
    test_users.add(int(line[0]))

print_time(start)


print("Counting the statistics...")

start = time.time()
max_id = 0
links_count = 0
mine_friends = defaultdict(dict)
graph_parts = [f for f in os.listdir(graph_path) if f.startswith("part")]

for file in graph_parts:
    csv_reader = csv.reader(open(os.path.join(graph_path, file)), delimiter='\t')

    for line in csv_reader:
        user = int(line[0])

        if user in test_users:
            for friendship in line[1][2:len(line[1]) - 2].split("),("):
                parts = friendship.split(",")
                mine_friends[user][int(parts[0])] = int(parts[1])

for (user, friends) in mine_friends.items():
    max_id = max(max_id, user)

    for friend in friends:
        links_count += 1
        max_id = max(max_id, friend)

print("Max id:", max_id, ",", links_count, " links")
print_time(start)


# load graph if it was not loaded yet
if not os.path.exists(os.path.join(results_path, "testGraph.npz")):
    print("Loading graph...")

    start = time.time()
    from_user = numpy.zeros(links_count, dtype=numpy.int32)
    to_user = numpy.zeros(links_count, dtype=numpy.int32)
    masks = numpy.zeros(links_count, dtype=numpy.int32)
    current = 0

    for file in graph_parts:
        print(file)

        csv_reader = csv.reader(open(os.path.join(graph_path, file)), delimiter='\t')

        for line in csv_reader:
            user = int(line[0])

            if user in test_users:
                for friendship in line[1][2:len(line[1]) - 2].split("),("):
                    parts = friendship.split(",")
                    from_user[current] = user - 1
                    to_user[current] = int(parts[0]) - 1
                    masks[current] = int(parts[1]) | 1
                    current += 1

        matrix = coo_matrix((masks, (from_user, to_user)), shape=(max_id, max_id)).tocsr()
        numpy.savez(os.path.join(results_path, "testGraph.npz"),
                    data=matrix.data,
                    indices=matrix.indices,
                    indptr=matrix.indptr,
                    shape=matrix.shape)

    print_time(start)


# load birth dates if they were not loaded yet
if not os.path.exists(os.path.join(results_path, "birthDates.npy")):
    print("Loading birth dates...")

    start = time.time()
    birth_dates = numpy.zeros(max_id, dtype=numpy.int32)
    demography_parts = [f for f in os.listdir(demography_path) if f.startswith("part")]

    for file in demography_parts:
        print(file)

        csv_reader = csv.reader(open(os.path.join(demography_path, file)), delimiter='\t')

        for line in csv_reader:
            user = int(line[0])
            birth_dates[user - 1] = int(line[2]) if line[2] != '' else 0

    numpy.save(os.path.join(results_path, "birthDates"), birth_dates)
    print_time(start)


# prediction
start = time.time()
graph_reloaded = load_csr(os.path.join(results_path, "testGraph"))
dates_reloaded = numpy.load(os.path.join(results_path, "birthDates.npy"))

print("Loading finished.")

with open(os.path.join(results_path, "prediction.csv"), 'w') as output:
    writer = csv.writer(output, delimiter=',')

    for user in test_users:
        ptr = graph_reloaded.indptr[user - 1]
        ptr_next = graph_reloaded.indptr[user]
        n = ptr_next - ptr

        data = graph_reloaded.data[ptr:ptr_next]
        indices = graph_reloaded.indices[ptr:ptr_next]

        dates = list(map(lambda x: float(dates_reloaded[x]), graph_reloaded.indices[ptr:ptr_next]))
        
        schoolmates = [float(dates_reloaded[indices[i]]) for i in range(0, n) if data[i] & 2 ** 10]
        university_fellows = [float(dates_reloaded[indices[i]]) for i in range(0, n) if data[i] & 2 ** 14]

        result = [mean(dates), median(dates)]

        if schoolmates:
            result.append(median(schoolmates))

        if university_fellows:
            result.append(median(university_fellows))

        writer.writerow([user, mean(result)])

    print_time(start)
