Version Unity = Unity 2022.3.62f2
Git command for work =
{
  # получить последние изменения
  git checkout main
  git pull origin main

  # создать ветку и работать
  git checkout -b feature/your-feature
  # после работы
  git add .
  git commit -m "feat: описания"
  git push origin feature/your-feature

  # создать PR на GitHub, после ревью мёрж
  # затем удалить локальную ветку
  git checkout main
  git pull origin main
  git branch -d feature/your-feature
}