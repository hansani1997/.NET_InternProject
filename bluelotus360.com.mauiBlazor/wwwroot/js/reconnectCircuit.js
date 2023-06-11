async function connectionDown(options) {

    console.log("Connection Down - you could do some UI here...");
    for (let i = 0; i < options.maxRetries; i++) {
        console.log("Waiting for reconnect attempt #" + (i + 1) + " ...");
        await this.delay(options.retryIntervalMilliseconds);
        if (this.isDisposed) {
            break;
        }

        try {
            // reconnectCallback will asynchronously return:
            // - true to mean success
            // - false to mean we reached the server, but it rejected the connection (e.g., unknown circuit ID)
            // - exception to mean we didn't reach the server (this can be sync or async)
            console.log("Starting Reconnect attempt #" + (i + 1) + " ...");
            const result = await window.Blazor.reconnect();
            if (!result) {
                // If the server responded and refused to reconnect, log it
                console.error("Server Rejected");
            } else {
                // Reconnected!
                return;
            }
        } catch (err) {
            // We got an exception so will try again
            console.error(err);
        }
    }
    // all attempts failed - let's try a full reload
    // This could be a UI change instead or something more complicated
    location.reload();
}

function delay(durationMilliseconds) {
    return new Promise(resolve => setTimeout(resolve, durationMilliseconds));
}

function connectionUp(e) {
    // Reconnected
    console.log("Connection UP!");
    // if you have a UI to hide/change you can do that here.
}

window.Blazor.start({
    reconnectionOptions: {
        maxRetries: 30,
        retryIntervalMilliseconds: 500,
    },
    reconnectionHandler: {
        onConnectionDown: e => connectionDown(e),
        onConnectionUp: e => connectionUp(e)
    }
});

