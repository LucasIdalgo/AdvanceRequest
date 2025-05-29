import type { IAdvanceRequest } from "../Interfaces/IAdvanceRequest";

export interface CreateAdvanceRequestProps {
  createAdvanceRequest: (contract:IAdvanceRequest) => Promise<boolean>;
}