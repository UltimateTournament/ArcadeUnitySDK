FROM gcr.io/distroless/cc
WORKDIR /game
COPY . .
USER 1000
ENV PORT=7778
ENTRYPOINT ["/game/LinuxServerBuild.x86_64", " -batchmode", "-nographics"]

