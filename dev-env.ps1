param (
  $command
)

function Main() {  
  Write-Host "Starting development environment"

  if ($command -eq "start") {
    docker-compose -f docker-compose.dev-env.yml up -d
  }
  elseif ($command -eq "stop") {
    docker-compose -f docker-compose.dev-env.yml down -v --rmi local --remove-orphans
  }  
}

Main