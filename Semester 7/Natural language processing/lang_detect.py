from keras import Sequential
from keras.layers import Dense, Dropout
from keras.models import load_model
from sklearn.metrics import classification_report
from preprocessing import *

data_directory = "./data/"

# region Neural network


def train_model(train_test_path, model_path):
    # Load train/test data
    train_test_data = np.load(train_test_path)
    x_train = train_test_data['X_train']
    y_train = train_test_data['y_train']

    print("x_train: ", x_train.shape)
    print("y_train: ", y_train.shape)

    del train_test_data

    # Create DNN
    model = Sequential()
    model.add(Dense(500, input_dim=vector_size, kernel_initializer="glorot_uniform", activation="sigmoid"))
    model.add(Dropout(0.5))
    model.add(Dense(300, kernel_initializer="glorot_uniform", activation="sigmoid"))
    model.add(Dropout(0.5))
    model.add(Dense(100, kernel_initializer="glorot_uniform", activation="sigmoid"))
    model.add(Dropout(0.5))
    model.add(Dense(len(language_codes), kernel_initializer="glorot_uniform", activation="softmax"))

    model_optimizer = keras.optimizers.Adam(lr=0.001, beta_1=0.9, beta_2=0.999, epsilon=1e-08, decay=0.0)
    model.compile(loss='categorical_crossentropy', optimizer=model_optimizer, metrics=['accuracy'])

    # Train
    model.fit(x_train, y_train,
              epochs=12,
              validation_split=0.10,
              batch_size=64,
              verbose=2,
              shuffle=True)

    model.save(model_path)


def test(train_test_path, model_path):
    train_test_data = np.load(train_test_path)
    x_test = train_test_data['X_test']
    y_test = train_test_data['y_test']

    print("x_test: ", x_test.shape)
    print("y_test: ", y_test.shape)

    del train_test_data

    model = load_model(model_path)
    scores = model.evaluate(x_test, y_test, verbose=1)

    print(f"{model.metrics_names[1]}: {(scores[1] * 100):4.4}%")

    # Scikit-learn classification report
    y_pred = model.predict_classes(x_test)
    y_pred = keras.utils.to_categorical(y_pred, num_classes=len(language_codes))

    print(classification_report(y_test, y_pred, target_names=language_codes))


# endregion

def prepare_model():
    vectors_path = os.path.join(data_directory, "samples", "sample_vectors.npz")
    train_test_path = os.path.join(data_directory, "train_test", "train_test_data.npz")
    model_path = "./model.h5"

    if not os.path.exists(os.path.join(data_directory, "samples")):
        os.makedirs(os.path.join(data_directory, "samples"))

    if not os.path.exists(os.path.join(data_directory, "train_test")):
        os.makedirs(os.path.join(data_directory, "train_test"))

    create_sample_vectors(data_directory, vectors_path)
    gen_train_test(vectors_path, train_test_path)
    train_model(train_test_path, model_path)
    test(train_test_path, model_path)


def evaluate_existing_model():
    train_test_path = os.path.join(data_directory, "train_test", "train_test_data.npz")
    model_path = os.path.join(data_directory, "model.h5")

    # create_sample_vectors(cleaned_data_directory, vectors_path)
    # gen_train_test(vectors_path, train_test_path)
    test(train_test_path, model_path)


prepare_model()
# evaluate_existing_model()
