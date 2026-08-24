import { Component, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ColaboradorService } from '../../core/services/colaborador.service';
import { UnidadeService } from '../../core/services/unidade.service';
import { UsuarioService } from '../../core/services/usuario.service';
import { Unidade } from '../../core/models/unidade.models';
import { Usuario } from '../../core/models/usuario.models';

@Component({
  selector: 'app-colaborador-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './colaborador-form.component.html',
})
export class ColaboradorFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly colaboradorService = inject(ColaboradorService);
  private readonly unidadeService = inject(UnidadeService);
  private readonly usuarioService = inject(UsuarioService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly idParam = this.route.snapshot.paramMap.get('id');
  readonly colaboradorId = this.idParam ? Number(this.idParam) : null;
  readonly isEdit = computed(() => this.colaboradorId !== null);

  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly todasUnidades = signal<Unidade[]>([]);
  readonly usuarios = signal<Usuario[]>([]);
  readonly usuarioAtualLogin = signal<string | null>(null);

  readonly unidadesSelecionaveis = computed(() =>
    this.isEdit() ? this.todasUnidades() : this.todasUnidades().filter((u) => u.ativo),
  );

  readonly form = this.fb.group({
    codigo: ['', Validators.required],
    nome: ['', Validators.required],
    unidadeId: [null as number | null, Validators.required],
    usuarioId: [null as number | null, Validators.required],
  });

  constructor() {
    this.unidadeService.list().subscribe((unidades) => this.todasUnidades.set(unidades));

    if (this.colaboradorId === null) {
      this.usuarioService.list(true).subscribe((usuarios) => this.usuarios.set(usuarios));
    } else {
      this.form.controls.codigo.disable();
      this.form.controls.usuarioId.disable();

      this.colaboradorService.getById(this.colaboradorId).subscribe((colaborador) => {
        this.usuarioAtualLogin.set(colaborador.usuarioLogin);
        this.form.patchValue({
          codigo: colaborador.codigo,
          nome: colaborador.nome,
          unidadeId: colaborador.unidadeId,
          usuarioId: colaborador.usuarioId,
        });
      });
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);

    const raw = this.form.getRawValue();

    const request$ = this.isEdit()
      ? this.colaboradorService.update(this.colaboradorId!, {
          nome: raw.nome!,
          unidadeId: raw.unidadeId!,
        })
      : this.colaboradorService.create({
          codigo: raw.codigo!,
          nome: raw.nome!,
          unidadeId: raw.unidadeId!,
          usuarioId: raw.usuarioId!,
        });

    request$.subscribe({
      next: () => this.router.navigate(['/colaboradores']),
      error: (err) => {
        this.errorMessage.set(err?.error?.detail ?? 'Não foi possível salvar o colaborador.');
        this.loading.set(false);
      },
    });
  }
}
