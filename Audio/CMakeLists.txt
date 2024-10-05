cmake_minimum_required(VERSION 3.4.1)

# Set the project name
project(EssentiaPlugin)

# Set the source file for compilation
set(SRC_FILES EssentiaPlugin.cpp)

# Include the path to the Essentia headers
include_directories(C:/Code/GuitAR/Assets/Plugins/Include)

# Add the library (shared library for Android)
add_library(libessentiaplugin SHARED ${SRC_FILES})

# Link the precompiled Essentia library and system libraries
find_library(log-lib log)
target_link_libraries(libessentiaplugin
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libessentia.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libfftw3.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libtag.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libsamplerate.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libavcodec.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libavformat.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libavutil.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libchromaprint.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libyaml.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libmp3lame.a
    ${CMAKE_SOURCE_DIR}/../Plugins/Android/libavresample.a
    log  # Required system library for Android logging
    android  # Required Android system library
    z  # Compression library used by some of the libraries (zlib)
)
