terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.31.0"
    }
  }

  required_version = "~> 1.0"
}

module "vpc" {
  source = "../vpc"
}

resource "aws_db_instance" "main" {
  allocated_storage       = "5"
  storage_type            = "standard"
  engine                  = "postgres"
  instance_class          = "db.t3.small"
  db_name                 = "HelloStory"
  username                = "foo"
  password                = "baregeogoevne"
  vpc_security_group_ids  = [module.vpc.aws_security_group_allow_all_id]
  db_subnet_group_name    = aws_db_subnet_group.main.id
  publicly_accessible = true
  apply_immediately       = true
  backup_retention_period = 0
  skip_final_snapshot     = true
}

resource "aws_db_subnet_group" "main" {
  subnet_ids = [module.vpc.aws_subnet_main_id, module.vpc.aws_subnet_second_id]
}
