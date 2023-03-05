import React, { useMemo, useState, useEffect } from 'react';
import { Column, useTable } from 'react-table';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import Moment from 'moment';

import { Row } from './models/Row';

const Table = () => {
  const [data, setData] = useState<Row[]>([]);
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
        .then(() => {
          console.log('Connected!');

          connection.on('Update', (rows: Row[]) => {
            setData(rows);
          });

          connection.on('Init', (rows: Row[]) => {
            setData(rows);
          });

          connection.invoke('Init');
        })
        .catch(e => console.log('Connection failed: ', e));
    }
  }, [connection]);

  const columns: Column<Row>[] = React.useMemo(
    () => [
      {
        Header: 'Id',
        accessor: 'id',
      },
      {
        Header: 'Ticker',
        accessor: 'ticker',
      },
      {
        Header: 'Last Value',
        accessor: 'lastValue',
      },
      {
        Header: 'Last Value Date',
        accessor: (row: Row) => Moment(row.lastValueDate)
          .local()
          .format("hh:mm:ss"),
      },
      {
        Header: 'Highest Buy Value',
        accessor: 'highestBuyValue',
      },
      {
        Header: 'Highest Buy Volume',
        accessor: 'highestBuyVolume',
      },
      {
        Header: 'Lowest Sell Value',
        accessor: 'lowestSellValue',
      },
      {
        Header: 'Lowest Sell Volume',
        accessor: 'lowestSellVolume',
      },
    ],
    []
  );

  const tableInstance = useTable({ columns, data });
  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    rows,
    prepareRow,
  } = tableInstance;

  return (
    <table {...getTableProps()}>
      <thead>
        {headerGroups.map(headerGroup => (
          <tr {...headerGroup.getFooterGroupProps()}>
            {headerGroup.headers.map(column => (
              <th {...column.getHeaderProps()}>
                {column.render('Header')}
              </th>
            ))}
          </tr>
        ))}
      </thead>
      <tbody {...getTableBodyProps()}>
        {rows.map(row => {
          prepareRow(row);
          return (
            <tr {...row.getRowProps()}>
              {
                row.cells.map(cell => {
                  return (
                    <td {...cell.getCellProps()}>
                      {
                        cell.render('Cell')}
                    </td>
                  )
                })}
            </tr>
          );
        })}
      </tbody>
    </table>
  );
};

export default Table;