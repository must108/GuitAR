LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

# Name of the output module/library
LOCAL_MODULE := libessentiaplugin

# Source file to compile
LOCAL_SRC_FILES := EssentiaPlugin.cpp

# Link to the Essentia library precompiled for Android
LOCAL_LDLIBS := -L$(LOCAL_PATH)/../../Assets/Plugins/Android -lessentia -llog -lstdc++

# Include directories for Essentia (located inside Unity project)
LOCAL_C_INCLUDES := C:/Code/GuitAR/Assets/Plugins/Include

# Build the shared library
include $(BUILD_SHARED_LIBRARY)
