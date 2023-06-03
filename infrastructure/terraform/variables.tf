
variable "digitalocean_token" {
  type = string
}

variable "digitalocean_region" {
  type    = string
  default = "ams3"
}

variable "application_name" {
  type    = string
  default = "dev"
}

variable "k8s" {
  type = object({
    version   = string
    node_size = string
    min_nodes = number
    max_nodes = number
  })
  default = {
    version   = "1.27.2-do.0"
    node_size = "s-2vcpu-2gb"
    min_nodes = 3
    max_nodes = 5
  }
}
