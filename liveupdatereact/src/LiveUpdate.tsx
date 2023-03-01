import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import React, { useEffect, useState } from 'react';

const LiveUpdate = () => {
    const [connection, setConnection] = useState<HubConnection | null>(null);

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/liveupdate')
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');

                    connection.on('Update', message => {
                        console.log(message);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    return null;
};

export default LiveUpdate;