FROM ubuntu:20.04
RUN apt-get update
RUN apt-get install openjdk-8-jre-headless -y
ADD styling.jar /
ADD opentelemetry-javaagent.jar /
CMD java -jar styling.jar
