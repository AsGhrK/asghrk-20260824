export interface Colaborador {
  id: number;
  codigo: string;
  nome: string;
  unidadeId: number;
  unidadeNome: string;
  usuarioId: number;
  usuarioLogin: string;
}

export interface ColaboradorCreateRequest {
  codigo: string;
  nome: string;
  unidadeId: number;
  usuarioId: number;
}

export interface ColaboradorUpdateRequest {
  nome: string;
  unidadeId: number;
}
