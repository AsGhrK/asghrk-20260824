import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ColaboradorService } from '../../core/services/colaborador.service';
import { Colaborador } from '../../core/models/colaborador.models';

@Component({
  selector: 'app-colaborador-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './colaborador-list.component.html',
})
export class ColaboradorListComponent {
  private readonly colaboradorService = inject(ColaboradorService);

  readonly colaboradores = signal<Colaborador[]>([]);
  readonly loading = signal(false);

  constructor() {
    this.carregar();
  }

  remover(colaborador: Colaborador): void {
    if (!confirm(`Remover o colaborador "${colaborador.nome}"?`)) {
      return;
    }
    this.colaboradorService.delete(colaborador.id).subscribe(() => this.carregar());
  }

  private carregar(): void {
    this.loading.set(true);
    this.colaboradorService.list().subscribe({
      next: (colaboradores) => {
        this.colaboradores.set(colaboradores);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }
}
