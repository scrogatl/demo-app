#FROM openjdk:8-jre
#FROM oyola/raspberry-pi4-debian-openjdk-8-jre
#FROM arm64v8/openjdk

FROM ubuntu:20.04
RUN apt-get update
RUN apt-get install openjdk-8-jre-headless -y
ADD shopping.jar /
CMD java -jar shopping.jar
