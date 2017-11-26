import numpy as np
from keras.callbacks import ModelCheckpoint
from keras.models import Sequential
from keras.layers import Dense, Dropout, Flatten, BatchNormalization
from keras.layers import Conv2D, MaxPooling2D
from keras.preprocessing.image import ImageDataGenerator
from keras.regularizers import l2
from keras.utils import np_utils
from keras.datasets import cifar10

classes = 10
epochs = 100
batch_size = 128
l2_lambda = 0.0001
kernel_size = (3, 3)
pool_size = (2, 2)

(x_train, y_train), (x_test, y_test) = cifar10.load_data()

x_train = x_train.astype('float32')
x_test = x_test.astype('float32')
x_train /= np.max(x_train)
x_test /= np.max(x_test)

y_train = np_utils.to_categorical(y_train, classes)
y_test = np_utils.to_categorical(y_test, classes)

datagen = ImageDataGenerator(width_shift_range=0.1, height_shift_range=0.1, horizontal_flip=True)
datagen.fit(x_train)

model = Sequential()

model.add(BatchNormalization(axis=1, input_shape=x_train.shape[1:]))

model.add(Conv2D(32, kernel_size, padding='same', activation='relu', kernel_regularizer=l2(l2_lambda)))
model.add(Dropout(0.25))
model.add(Conv2D(32, kernel_size, padding='same', activation='relu', kernel_regularizer=l2(l2_lambda)))
model.add(MaxPooling2D(pool_size=pool_size))

model.add(Conv2D(64, kernel_size, padding='same', activation='relu', kernel_regularizer=l2(l2_lambda)))
model.add(Dropout(0.25))
model.add(Conv2D(64, kernel_size, padding='same', activation='relu', kernel_regularizer=l2(l2_lambda)))
model.add(MaxPooling2D(pool_size=pool_size))

model.add(Conv2D(128, kernel_size, padding='same', activation='relu', kernel_regularizer=l2(l2_lambda)))
model.add(Dropout(0.25))
model.add(Conv2D(128, kernel_size, padding='same', activation='relu', kernel_regularizer=l2(l2_lambda)))
model.add(MaxPooling2D(pool_size=pool_size))

model.add(Flatten())

model.add(Dense(1024, activation='relu', kernel_regularizer=l2(l2_lambda)))
model.add(Dropout(0.5))
model.add(Dense(512, activation='relu', kernel_regularizer=l2(l2_lambda)))
model.add(Dropout(0.25))
model.add(Dense(classes, activation='softmax'))

model.compile(loss='categorical_crossentropy', optimizer='adam', metrics=['accuracy'])

model.fit_generator(datagen.flow(x_train, y_train, batch_size=batch_size),
                    steps_per_epoch=int(np.ceil(x_train.shape[0] / float(batch_size))),
                    epochs=epochs,
                    verbose=2,
                    callbacks=[ModelCheckpoint("model-{epoch:03d}-{val_acc:.4f}.h5", monitor='val_acc')],
                    validation_data=(x_test, y_test))

score = model.evaluate(x_test, y_test, verbose=0)

print("Final results")
print("Loss:", score[0])
print("Accuracy:", score[1])

model.save("model.h5")

# Accuracy: 0.8776
