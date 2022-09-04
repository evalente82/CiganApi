# CiganApi
Api de Lista de tarefas

Desenvolva uma API RESTful que cadastre, consulte e altere em uma lista 'Tarefas' definidas por um usuário.

Propriedades da Lista
Codigo
Descricao
Status (P = Pendente, C = Concluído)

Regras de Negócio
Inclusão
- não deve repetir códigos => OK
- toda nova Tarefa cadastrada deve estar com o status 'P'
- sempre deve ter uma descrição definida => OK

Alteração
- Não é permitido alterar o código => ok
- a descrição tem que estar definida => ok
- o Status só pode ser passado para 'C' => ok

Consulta
- Total- Retorne um JSON com as informações de todas as Tarefas => ok
- Específica - Retorne um JSON com as informações da Tarefa que foi passado o código por parâmetro. => ok
