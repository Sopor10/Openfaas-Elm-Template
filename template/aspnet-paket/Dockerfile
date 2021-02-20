FROM alpine:3.13 as watchdog

RUN apk --no-cache add curl \
    && echo "Pulling watchdog binary from Github." \
    && curl -sSL https://github.com/openfaas-incubator/of-watchdog/releases/download/0.5.1/of-watchdog > /usr/bin/fwatchdog \
    && chmod +x /usr/bin/fwatchdog \
    && cp /usr/bin/fwatchdog /home/app \
    && apk del curl --no-cache

FROM mcr.microsoft.com/dotnet/sdk:5.0 as builder

# Supress collection of data.
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Optimize for Docker builder caching by adding projects first.

RUN mkdir -p /home/app/function
RUN mkdir -p /home/app/function/Function
WORKDIR /home/app


#COPY root.sln ./
#COPY root.csproj ./


WORKDIR /home/app
COPY .  .

WORKDIR /home/app/function
RUN dotnet tool restore
RUN dotnet paket restore 

WORKDIR /home/app
RUN dotnet publish ./function/Function/Function.csproj -o published

FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY --from=watchdog /usr/bin/fwatchdog  /usr/bin/

# Create a non-root user
RUN addgroup --system app \
    && adduser --system --ingroup app app

WORKDIR /home/app/
COPY --from=builder /home/app/published .
RUN chown app:app -R /home/app

USER app

ENV fprocess="dotnet ./Function.dll"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost:5000"
EXPOSE 8080

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]