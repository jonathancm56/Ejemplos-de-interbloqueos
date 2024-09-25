import networkx as nx
import matplotlib.pyplot as plt

# Crear un nuevo grafo dirigido
G = nx.DiGraph()

# Añadir nodos (procesos y recursos)
nodos = ['Proceso 1', 'Recurso 1', 'Proceso 2', 'Recurso 2']
G.add_nodes_from(nodos)

# Añadir aristas de la solución sin interbloqueo
edges = [('Proceso 1', 'Recurso 1'), ('Recurso 1', 'Proceso 2'), ('Proceso 2', 'Recurso 2')]  # Eliminar el ciclo ('Recurso 2', 'Proceso 1')
G.add_edges_from(edges)

# Dibujar el grafo
pos = nx.circular_layout(G)  # Usar el mismo diseño circular
nx.draw(G, pos, with_labels=True, node_color='lightgreen', font_weight='bold', node_size=3000, font_size=10)

# Dibujar etiquetas de las aristas (opcional)
edge_labels = {edge: 'dep' for edge in edges}
nx.draw_networkx_edge_labels(G, pos, edge_labels=edge_labels)

# Mostrar el grafo en pantalla
plt.title("Solución: Sin interbloqueo")
plt.show()
