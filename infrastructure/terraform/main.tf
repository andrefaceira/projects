
# resource "digitalocean_vpc" "default" {
#   name   = "${var.application_name}-vpc"
#   region = var.digitalocean_region
# }

resource "digitalocean_kubernetes_cluster" "default" {
  name     = "${var.application_name}-k8s"
  # vpc_uuid = digitalocean_vpc.default.id
  region   = var.digitalocean_region
  version  = var.k8s.version

  node_pool {
    name       = "${var.application_name}-k8s-pool-${var.k8s.node_size}"
    size       = var.k8s.node_size
    auto_scale = true
    min_nodes  = var.k8s.min_nodes
    max_nodes  = var.k8s.max_nodes

    labels = {
      size = var.k8s.node_size
    }

    tags = [
      var.application_name
    ]
  }

  # auto_upgrade = true
  # maintenance_policy {
  #   start_time = "04:00"
  #   day        = "any"
  # }

  tags = [
    var.application_name
  ]
}

output k8s_id {
  value = digitalocean_kubernetes_cluster.default.id
}