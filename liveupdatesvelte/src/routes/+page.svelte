<script lang="ts">
    import { HubConnectionBuilder } from '@microsoft/signalr';
    import {
        flexRender,
        createSvelteTable,
        createColumnHelper,
        getCoreRowModel
    } from '@tanstack/svelte-table';
    import type { TableOptions } from '@tanstack/svelte-table';
    import { onMount, onDestroy } from 'svelte';
    import { writable } from 'svelte/store';
    import Moment from 'moment';

    import type { Row } from '../models/Row';

    const columnHelper = createColumnHelper<Row>();

    const columns = [
        columnHelper.accessor('id', {
            header: 'Id'
        }),
        columnHelper.accessor('ticker', {
            header: 'Ticker'
        }),
        columnHelper.accessor('lastValue', {
            header: 'Last Value'
        }),
        columnHelper.accessor('lastValueDate', {
            header: 'Last Value Date',
            cell: (props) => Moment(props.getValue()).local().format('hh:mm:ss')
        }),
        columnHelper.accessor('highestBuyValue', {
            header: 'Highest Buy Value'
        }),
        columnHelper.accessor('highestBuyVolume', {
            header: 'Highest Buy Volume'
        }),
        columnHelper.accessor('lowestSellValue', {
            header: 'Lowest Sell Value'
        }),
        columnHelper.accessor('lowestSellVolume', {
            header: 'Lowest Sell Volume'
        })
    ];

    const options = writable<TableOptions<Row>>({
        data: [],
        columns: columns,
        getCoreRowModel: getCoreRowModel()
    });

    const connection = new HubConnectionBuilder()
        .withUrl('https://localhost:5001/liveupdate')
        .withAutomaticReconnect()
        .build();

    onMount(async () => {
        await connection.start().then(() => {
            console.log('Connected!');

            connection.on('Update', (rows: Row[]) => {
                options.update((options) => ({
                    ...options,
                    data: rows
                }));
            });

            connection.on('Init', (rows: Row[]) => {
                options.update((options) => ({
                    ...options,
                    data: rows
                }));
            });

            connection.invoke('Init');
        });
    });

    onDestroy(async () => {
        await connection.stop();
    });

    const table = createSvelteTable(options);
</script>

<table>
    <thead>
        {#each $table.getHeaderGroups() as headerGroup}
            <tr>
                {#each headerGroup.headers as header}
                    <th colSpan={header.colSpan}>
                        {#if !header.isPlaceholder}
                            <svelte:component
                                this={flexRender(
                                    header.column.columnDef.header,
                                    header.getContext()
                                )}
                            />
                        {/if}
                    </th>
                {/each}
            </tr>
        {/each}
    </thead>
    <tbody>
        {#each $table.getRowModel().rows as row}
            <tr>
                {#each row.getVisibleCells() as cell}
                    <td>
                        <svelte:component
                            this={flexRender(cell.column.columnDef.cell, cell.getContext())}
                        />
                    </td>
                {/each}
            </tr>
        {/each}
    </tbody>
</table>
