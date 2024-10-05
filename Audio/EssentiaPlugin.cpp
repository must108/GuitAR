#include <essentia/algorithmfactory.h>
#include <essentia/streaming/algorithms/chromagram.h>
#include <essentia/streaming/algorithms/pitchyinfft.h>
#include <essentia/streaming/algorithms/onsetdetection.h>
#include <essentia/streaming/algorithms/energy.h>
#include <essentia/streaming/source.h>
#include <essentia/streaming/sink.h>

extern "C" {
    using namespace essentia;
    using namespace essentia::streaming;

    void initEssentia() {
        essentia::init();
    }

    Algorithm* createStreamingAlgorithm(const char* name) {
        Algorithm* alg = AlgorithmFactory::create(name);
        return alg;
    }

    void configurePitchYinFFT(Algorithm* pitchAlgorithm, float tolerance) {
        pitchAlgorithm->configure("tolerance", tolerance);
    }

    void connect(SourceBase* source, SinkBase* sink) {
        source->output.connect(sink->input);
    }

    void processBuffer(Algorithm* algorithm) {
        algorithm->compute();
    }

    void deleteStreamingAlgorithm(Algorithm* algorithm) {
        delete algorithm;
    }

    void shutdownEssentia() {
        essentia::shutdown();
    }
}
