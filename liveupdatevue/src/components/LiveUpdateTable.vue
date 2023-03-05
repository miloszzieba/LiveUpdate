<script setup lang="ts">
    import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
    import {
        FlexRender,
        getCoreRowModel,
        useVueTable,
        createColumnHelper,
    } from '@tanstack/vue-table';
    import Moment from 'moment';
    import { ref, onMounted } from 'vue';
    import type { Ref } from 'vue';

    import { Row } from "../models/Row";

    const rows: Ref<Row[]> = ref<Row[]>([]);

    onMounted(() => {
        const connection: HubConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/liveupdate')
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(() => {
                console.log('Connected!');

                connection.on('Update', (data: Row[]) => {
                    rows.value = data;
                });

                connection.on('Init', (data: Row[]) => {
                    rows.value = data;
                });

                connection.invoke('Init');
            })
            .catch(e => console.log('Connection failed: ', e));
    });

    const columnHelper = createColumnHelper<Row>();

    const columns = [
        columnHelper.accessor("id", {
            header: 'Id',
        }),
        columnHelper.accessor("ticker", {
            header: 'Ticker',
        }),
        columnHelper.accessor("lastValue", {
            header: 'Last Value',
        }),
        columnHelper.accessor("lastValueDate", {
            header: 'Last Value Date',
            cell: (props) => Moment(props.getValue())
                .local()
                .format("hh:mm:ss")
        }),
        columnHelper.accessor("highestBuyValue", {
            header: 'Highest Buy Value',
        }),
        columnHelper.accessor("highestBuyVolume", {
            header: 'Highest Buy Volume',
        }),
        columnHelper.accessor("lowestSellValue", {
            header: 'Lowest Sell Value',
        }),
        columnHelper.accessor("lowestSellVolume", {
            header: 'Lowest Sell Volume',
        }),
    ];

    const table = useVueTable({
        get data() {
            return rows.value;
        },
        columns,
        getCoreRowModel: getCoreRowModel()
    });
</script>

<template>
    <table>
        <thead>
            <tr v-for="headerGroup in table.getHeaderGroups()"
                :key="headerGroup.id">
                <th v-for="header in headerGroup.headers"
                    :key="header.id"
                    :colSpan="header.colSpan">
                    <FlexRender v-if="!header.isPlaceholder"
                                :render="header.column.columnDef.header"
                                :props="header.getContext()" />
                </th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="row in table.getRowModel().rows" :key="row.id">
                <td v-for="cell in row.getVisibleCells()" :key="cell.id">
                    <FlexRender :render="cell.column.columnDef.cell"
                                :props="cell.getContext()" />
                </td>
            </tr>
        </tbody>
    </table>
</template>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
