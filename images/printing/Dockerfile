FROM ubuntu:20.04
RUN apt-get update
RUN apt-get install openjdk-11-jre-headless -y
ADD printing.jar /
ADD opentelemetry-javaagent.jar /
CMD java -jar printing.jar
