export interface IInstallments {
  InstallmentId: number;
  ContractId: number;
  DueDate: Date;
  Amount: number;
  Status: string;
  Antecipated: boolean;
}