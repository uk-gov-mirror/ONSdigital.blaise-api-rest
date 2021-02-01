# blaise-api-rest

## Mono

Can we run the rest-api in a linux container on mono?

### Current status

Works, but API responses seem to be extremely slow.

### Build

```sh
rm -rf packages
docker run -it -v $PWD:/tmp/app mono:6.12 /bin/bash -c "nuget sources add -Name 'azuredevops' -Source 'https://pkgs.dev.azure.com/blaise-gcp/csharp/_packaging/CSharp%40Local/nuget/v3/index.json' -username ' ' -password '${AZURE_DEVOPS_TOKEN}' && cd /tmp/app && nuget restore *.sln && msbuild *.sln"
```

### RUN

```sh
docker build . -f docker/run/Dockerfile -t blaise-rest-api
```

```sh
docker run -it -p 127.0.0.1:8080:90/tcp \
  -e ENV_BLAISE_SERVER_HOST_NAME=dev-web.social-surveys.gcp.onsdigital.uk \
  -e ENV_BLAISE_SERVER_BINDING=HTTPS \
  -e ENV_BLAISE_ADMIN_USER=blaise \
  -e ENV_BLAISE_ADMIN_PASSWORD=${BLAISE_ADMIN_PASSWORD} \
  -e ENV_BLAISE_CONNECTION_PORT=8031 \
  -e ENV_BLAISE_REMOTE_CONNECTION_PORT=8033 \
  -e ENV_LIBRARY_DIRECTORY=/Blaise5/Surveys \
  -e ENV_CONNECTION_EXPIRES_IN_MINUTES=60 \
  -e ENV_DB_CONNECTIONSTRING="" \
  blaise-rest-api
```

```sh
curl localhost:8080/api/v1/health
```
