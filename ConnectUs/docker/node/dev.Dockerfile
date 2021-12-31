FROM node:17.3.0 AS node
COPY . /src
WORKDIR /src/ConnectUs
RUN npm install
RUN apt-get update && apt-get install dos2unix
RUN chmod +x docker/node/entrypoint.sh
RUN dos2unix docker/node/entrypoint.sh
ENTRYPOINT ["sh", "docker/node/entrypoint.sh"]