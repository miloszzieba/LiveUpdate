export type Row = {
    id: number;
    ticker: string;
    lastValue: number;
    lastValueDate: Date;
    highestBuyValue: number;
    highestBuyVolume: number;
    lowestSellValue: number;
    lowestSellVolume: number;
};
