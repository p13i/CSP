FROM ubuntu

MAINTAINER Pramod Kotipalli

# Install mono
RUN apt-get update -y \
    && apt-get install -y mono-complete
