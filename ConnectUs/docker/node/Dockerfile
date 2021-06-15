FROM node:lts AS node
RUN cd home
ARG CACHE_DATE=2016-01-01
RUN ls
COPY . /src
RUN cd src
RUN cd ConnectUs
WORKDIR /src/ConnectUs
RUN npm install
RUN apt-get update && apt-get install dos2unix
RUN chmod +x entrypoint.sh
RUN dos2unix entrypoint.sh
ENTRYPOINT ["sh", "entrypoint.sh"]