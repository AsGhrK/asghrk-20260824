export interface Usuario {
  id: number;
  codigo: string;
  login: string;
  ativo: boolean;
}

export interface UsuarioCreateRequest {
  codigo: string;
  login: string;
  senha: string;
}

export interface UsuarioUpdateRequest {
  senha?: string | null;
  ativo?: boolean | null;
}
