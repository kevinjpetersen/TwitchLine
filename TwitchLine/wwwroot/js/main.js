var TwitchEmbed = null;
var TwitchEmbbed = false;
var ForcedStop = true;

window.TwitchEmbedStart = (channelName) => {
    TwitchEmbbed = false;
    TwitchEmbed = new Twitch.Embed("twitchline-view", {
        channel: channelName,
        layout: "video",
        height: "100%",
        width: "100%"
    });

    TwitchEmbed.addEventListener(Twitch.Embed.VIDEO_PLAY, () => {
        if (TwitchEmbbed === false || ForcedStop === true) {
            TwitchEmbbed = true;
            setTimeout(() => {
                TwitchEmbed.player.pause();
            }, 500);
        }
    });
};

window.TwitchPlayerSetChannel = (channelName) => {
    TwitchEmbed.player.setChannel(channelName);
};

window.TwitchPlayerStop = () => {
    ForcedStop = true;
    TwitchEmbed.player.pause();
    TwitchEmbed.player.setMuted(true);
};

window.TwitchPlayerPlay = () => {
    ForcedStop = false;
    TwitchEmbed.player.play();
    TwitchEmbed.player.setMuted(false);
};

window.ScrollOutputToBottom = (outputId) => {
    var outputArea = document.getElementById(outputId);
    outputArea.scrollTop = outputArea.scrollHeight;
};