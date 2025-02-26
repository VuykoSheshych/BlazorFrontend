window.addBeforeUnloadListener = (dotNetHelper) => {
    window.addEventListener("beforeunload", () => {
        dotNetHelper.invokeMethodAsync("TerminateGameSearch");
    });
};