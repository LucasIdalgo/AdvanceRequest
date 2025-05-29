export interface LoginProps {
  onLogin: (email: string, password: string) => Promise<boolean>;
}