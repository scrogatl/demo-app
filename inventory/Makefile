# BUILDARCH is the host architecture
# ARCH is the target architecture
# we need to keep track of them separately
BUILDARCH ?= $(shell uname -m)

# canonicalized names for host architecture
ifeq ($(BUILDARCH),aarch64)
        BUILDARCH=arm64
endif
ifeq ($(BUILDARCH),x86_64)
        BUILDARCH=amd64
endif
ifeq ($(BUILDARCH),armv7l)
        BUILDARCH=armv7
endif

# unless otherwise set, I am building for my own architecture, i.e. not cross-compiling
ARCH ?= $(BUILDARCH)

# canonicalized names for target architecture
ifeq ($(ARCH),aarch64)
        override ARCH=arm64
endif
ifeq ($(ARCH),armv7l)
        override ARCH=armv7
endif
ifeq ($(ARCH),x86_64)
        override ARCH=amd64
endif
OUT_DIR?=./target
GOLANG_VERSION?=1.11

BINARY_NAME=inventory

ifndef TEMP_DIR
TEMP_DIR:=$(shell mktemp -d /tmp/wavefront.XXXXXX)
endif

VERSION?=2.6.2
GIT_COMMIT:=$(shell git rev-parse --short HEAD)

REPO_DIR:=$(shell pwd)

LDFLAGS=-w -X main.version=$(VERSION) -X main.commit=$(GIT_COMMIT)

all: build

fmt:
	find . -type f -name "*.go" | grep -v "./vendor*" | xargs gofmt -s -w

build: clean fmt
	GOARCH=$(ARCH) CGO_ENABLED=0 go build -ldflags "$(LDFLAGS)" -o $(OUT_DIR)/$(ARCH)/$(BINARY_NAME) ./cmd/inventory/

build-linux: clean fmt
	GOARCH=$(ARCH) GOOS=linux CGO_ENABLED=0 go build -ldflags "$(LDFLAGS)" -o $(OUT_DIR)/$(ARCH)/$(BINARY_NAME) ./cmd/inventory/

clean:
	rm -f $(BINARY_NAME)

.PHONY: all fmt container clean
