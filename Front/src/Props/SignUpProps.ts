export interface SignUpProps {
  onRegister: (name: string, email: string, password: string) => Promise<boolean>;
}