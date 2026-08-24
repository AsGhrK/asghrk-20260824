export interface ColaboradorResumo {
  id: number;
  codigo: string;
  nome: string;
}

export interface Unidade {
  id: number;
  codigo: string;
  nome: string;
  ativo: boolean;
  colaboradores: ColaboradorResumo[];
}

export interface UnidadeCreateRequest {
  codigo: string;
  nome: string;
}

export interface UnidadeUpdateRequest {
  ativo: boolean;
}
