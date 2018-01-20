from keras import Sequential
from keras.layers import Dense, Dropout, Conv1D, MaxPooling1D, Flatten
from keras.models import load_model
from sklearn.metrics import classification_report
from preprocessing import *

data_directory = "./data/"
model_path = "./model.h5"


def train_model(train_test_path):
    """
    Creates a model and performs training.
    """
    # Load train/test data
    train_test_data = np.load(train_test_path)
    x_train = train_test_data['X_train']
    y_train = train_test_data['y_train']

    print("x_train:", x_train.shape)
    print("y_train:", y_train.shape)

    del train_test_data

    x_train = np.expand_dims(x_train, axis=3)

    # Create network
    model = Sequential()
    model.add(Conv1D(128, 5, input_shape=x_train.shape[1:], padding='same', activation='relu'))
    model.add(MaxPooling1D(5))
    model.add(Conv1D(128, 5, padding='same', activation='relu'))
    model.add(MaxPooling1D(5))
    model.add(Dropout(0.5))

    model.add(Flatten())

    model.add(Dense(1024, kernel_initializer='glorot_uniform', activation='relu'))
    model.add(Dropout(0.5))
    model.add(Dense(512, kernel_initializer='glorot_uniform', activation='relu'))
    model.add(Dropout(0.5))
    model.add(Dense(256, kernel_initializer='glorot_uniform', activation='relu'))
    model.add(Dropout(0.5))
    model.add(Dense(128, kernel_initializer='glorot_uniform', activation='relu'))
    model.add(Dropout(0.5))
    model.add(Dense(len(language_codes), kernel_initializer='glorot_uniform', activation='softmax'))

    model_optimizer = keras.optimizers.Adam(lr=0.001, beta_1=0.9, beta_2=0.999, epsilon=1e-08, decay=0.0)
    model.compile(loss='categorical_crossentropy', optimizer=model_optimizer, metrics=['accuracy'])

    # Train
    model.fit(x_train, y_train,
              epochs=10,
              validation_split=0.10,
              batch_size=64,
              verbose=2,
              shuffle=True)

    model.save(model_path)


def test(train_test_path):
    """
    Loads model and validates it on given data.
    """
    train_test_data = np.load(train_test_path)
    x_test = train_test_data['X_test']
    y_test = train_test_data['y_test']

    print("x_test: ", x_test.shape)
    print("y_test: ", y_test.shape)

    del train_test_data

    x_test = np.expand_dims(x_test, axis=3)

    model = load_model(model_path)
    scores = model.evaluate(x_test, y_test, verbose=1)

    print(f"{model.metrics_names[1]}: {(scores[1] * 100):4.4}%")

    # Scikit-learn classification report
    y_pred = model.predict_classes(x_test)
    y_pred = keras.utils.to_categorical(y_pred, num_classes=len(language_codes))

    print(classification_report(y_test, y_pred, target_names=language_codes))


def start(model_exists=False):
    """
    Starts the process of training/testing.
    """
    vectors_directory = os.path.join(data_directory, "samples")
    vectors_path = os.path.join(vectors_directory, "sample_vectors.npz")
    train_test_directory = os.path.join(data_directory, "train_test")
    train_test_path = os.path.join(train_test_directory, "train_test_data.npz")

    if not os.path.exists(vectors_directory):
        os.makedirs(vectors_directory)

    if not os.path.exists(train_test_directory):
        os.makedirs(train_test_directory)

    create_sample_vectors(data_directory, vectors_path)
    gen_train_test(vectors_path, train_test_path)

    if not model_exists:
        train_model(train_test_path)

    test(train_test_path)


start()
