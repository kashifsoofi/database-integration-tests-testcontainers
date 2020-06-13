FROM dotnetroundhouse/roundhouse

ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet tool install -g dotnet-script

WORKDIR /db

# Copy all db files
COPY . .

ENTRYPOINT [ "dotnet-script", "Detector.csx", "--", "rh", "--silent", "-dt", "mysql" ]
CMD ["-cs", "server=host.docker.internal;Port=3306;database=defaultdb;uid=root;password=Password123;SslMode=None;"]
