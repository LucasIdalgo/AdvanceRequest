export interface IAdvanceRequest {
  AdvanceRequestId: number;
  ContractId: number;
  InstallmentQuantity: number;
  ClientId: number;
  Approve: boolean;
  ApprovetAt: Date | null;
  CreatedAt: Date;
}