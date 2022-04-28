docker run \
    -e WAVEFRONT_URL=https://longboard.wavefront.com/api \
    -e WAVEFRONT_TOKEN="d6bb5460-02ff-438e-bc9c-9b321ad84093" \
    -p 2878:2878 \
    dambott/arm64proxy:latest
