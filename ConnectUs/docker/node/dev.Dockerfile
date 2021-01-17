FROM node:lts AS node
COPY . /src
WORKDIR /src/ConnectUs
RUN npm install
RUN chmod +x docker/node/entrypoint.sh
CMD /bin/bash docker/node/entrypoint.sh
