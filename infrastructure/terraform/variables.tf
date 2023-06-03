
variable "digitalocean_token" {
  type = string
  default = "dop_v1_9701b31b1b12b2ff31d7b33032219afeb5fc6e6ab9a662b5d0a900354017acb7"
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
