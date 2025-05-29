import type { IContract } from "../Interfaces/IContract";

export interface ContractProps {
  loadContracts: (page: number, limit: number) => Promise<IContract[]>;
}