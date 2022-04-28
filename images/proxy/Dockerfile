# This script may automatically configure wavefront without prompting, based on
# these variables:
#  WAVEFRONT_URL           (required)
#  WAVEFRONT_TOKEN         (required)
#  JAVA_HEAP_USAGE         (default is 4G)
#  WAVEFRONT_HOSTNAME      (default is the docker containers hostname)
#  WAVEFRONT_PROXY_ARGS    (default is none)
#  JAVA_ARGS               (default is none)

FROM ubuntu:20.04
RUN apt-get update
RUN apt-get install openjdk-8-jre-headless -y
ADD proxy.jar proxy.jar
ADD wavefront.conf wavefront.conf
ADD log4j2-stdout.xml.default /etc/wavefront/wavefront-proxy/log4j2.xml
EXPOSE 2878
EXPOSE 30000
EXPOSE 40000
EXPOSE 4317
CMD java -jar proxy.jar -f wavefront.conf
