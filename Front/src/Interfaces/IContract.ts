import type { IInstallments } from "./IInstallments";

export interface IContract{
  ContractId:number;
  ClientId:number;
  ClientName: string;
  Active:boolean;
  Installments: IInstallments[]
}