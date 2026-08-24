import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UnidadeService } from '../../core/services/unidade.service';
import { Unidade } from '../../core/models/unidade.models';

@Component({
  selector: 'app-unidade-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './unidade-list.component.html',
})
export class UnidadeListComponent {
  private readonly unidadeService = inject(UnidadeService);

  readonly unidades = signal<Unidade[]>([]);
  readonly loading = signal(false);
  readonly expandedId = signal<number | null>(null);

  constructor() {
    this.carregar();
  }

  toggleExpand(id: number): void {
    this.expandedId.set(this.expandedId() === id ? null : id);
  }

  alternarStatus(unidade: Unidade): void {
    this.unidadeService.updateStatus(unidade.id, { ativo: !unidade.ativo }).subscribe(() => this.carregar());
  }

  private carregar(): void {
    this.loading.set(true);
    this.unidadeService.list().subscribe({
      next: (unidades) => {
        this.unidades.set(unidades);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }
}
