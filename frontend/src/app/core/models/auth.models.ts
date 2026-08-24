export interface LoginRequest {
  login: string;
  senha: string;
}

export interface TokenResponse {
  token: string;
  expiresAt: string;
}
