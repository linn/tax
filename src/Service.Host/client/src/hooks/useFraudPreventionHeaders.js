import { useState, useEffect } from 'react';

function getLocalIPs(callback) {
    const ipList = [];
    const RTCPeerConnection =
        window.RTCPeerConnection || window.webkitRTCPeerConnection || window.mozRTCPeerConnection;

    const connection = new RTCPeerConnection({
        iceServers: []
    });
    connection.createDataChannel('');

    connection.onicecandidate = e => {
        if (!e.candidate) {
            connection.close();
            callback(ipList);
            return;
        }
        const ip = /^candidate:.+ (\S+) \d+ typ/.exec(e.candidate.candidate)[1];
        ipList.push(ip);
    };

    connection.createOffer(
        sdp => {
            connection.setLocalDescription(sdp);
        },
        () => {}
    );
}

function useFraudPreventionHeaders(username) {
    const [fraudPreventionHeaders, setFraudPreventionHeaders] = useState({
        doNotTrack: !!navigator?.doNotTrack,
        windowWidth: window.innerWidth,
        windowHeight: window.innerHeight,
        browserPlugins: null,
        userAgentString: navigator.userAgent,
        localIps: null,
        localIpsTimestamp: null,
        screenWidth: window.screen.width,
        screenHeight: window.screen.height,
        scalingFactor: window.devicePixelRatio,
        colourDepth: window.screen.colorDepth,
        timezoneOffset: new Date().getTimezoneOffset(),
        username
    });

    useEffect(() => {
        const plugins = [];
        for (let i = 0; i < navigator.plugins.length; i += 1) {
            plugins.push(navigator.plugins[i]);
        }
        setFraudPreventionHeaders(r => ({ ...r, browserPlugins: plugins.map(p => p.name) }));
        getLocalIPs(ipList =>
            setFraudPreventionHeaders(r => ({
                ...r,
                localIps: ipList,
                localIpsTimestamp: new Date().toISOString()
            }))
        );
    }, [setFraudPreventionHeaders]);

    function windowSize() {
        setFraudPreventionHeaders(f => ({
            ...f,
            windowHeight: window.innerHeight,
            windowWidth: window.innerWidth
        }));
    }

    window.onresize = windowSize;

    return fraudPreventionHeaders;
}

export default useFraudPreventionHeaders;
