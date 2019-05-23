# jeikabu/debian-build-nng:arm64v8-stretch
# Build NNG ARM64
# Volumes:
# /usr/src = build output
# Example:
# docker build --build-arg NNG_BRANCH=v1.1.1 -t debian-build-nng:arm64v8-stretch-v1.1.1
# mkdir -p /tmp/binary-output
# docker run --rm -t -v /tmp/binary-output:/usr/src debian-build-nng:arm64v8-stretch-v1.1.1

FROM jeikabu/debian-build:arm64v8-stretch

ARG NNG_BRANCH=HEAD

RUN git clone https://github.com/nanomsg/nng.git \
    && cd nng && git checkout $NNG_BRANCH

CMD cd /usr/src \
    && cmake -G "Unix Makefiles" -DBUILD_SHARED_LIBS=ON -DCMAKE_BUILD_TYPE=Release -DNNG_TESTS=OFF -DNNG_TOOLS=OFF /nng \
    && make