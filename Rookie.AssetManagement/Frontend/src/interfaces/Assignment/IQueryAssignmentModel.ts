export default interface IQueryAssetModel {
    page: number;
    limit: number;
    assignDate: Date;
    states:string[];
    search: string;
    sortOrder: string;
    sortColumn: string;
  }
  