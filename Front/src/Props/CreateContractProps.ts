import type { IContract } from "../Interfaces/IContract";

export interface CreateContractProps {
  createContracts: (contract:IContract) => Promise<boolean>;
}